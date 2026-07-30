using System.Text.Json;
using Research.Shared.Articles;

namespace Research.Labs.Articles;

public class BuildingDatabaseInTextFileAsyncTests
{
    [Fact]
    public async Task NaiveFileStorage_TwoConcurrentWrites_ReadsOverlap_AndDataIsLost()
    {
        var mockFileSystem = new ReadConcurrencyGateFileSystem(requiredConcurrentReaders: 2);
        var storage = new BuildingDatabaseInTextFileAsync.NaiveFileStorage("test.json", mockFileSystem);

        var writeA = storage.WriteAsync(new BuildingDatabaseInTextFileAsync.FileRecord(Guid.NewGuid(), "A", "dataA", DateTime.UtcNow));
        var writeB = storage.WriteAsync(new BuildingDatabaseInTextFileAsync.FileRecord(Guid.NewGuid(), "B", "dataB", DateTime.UtcNow));
        await Task.WhenAll(writeA, writeB);

        Assert.True(mockFileSystem.AllReadersArrivedConcurrently);
        Assert.Single(mockFileSystem.GetAllRecords());
    }

    [Fact]
    public async Task LessNaiveFileStorage_TwoConcurrentWrites_AreSerialized_AndNoDataIsLost()
    {
        var mockFileSystem = new ReadConcurrencyGateFileSystem(requiredConcurrentReaders: 2);
        var storage = new BuildingDatabaseInTextFileAsync.LessNaiveFileStorage("test.json", mockFileSystem);

        var writeA = storage.WriteAsync(new BuildingDatabaseInTextFileAsync.FileRecord(Guid.NewGuid(), "A", "dataA", DateTime.UtcNow));
        var writeB = storage.WriteAsync(new BuildingDatabaseInTextFileAsync.FileRecord(Guid.NewGuid(), "B", "dataB", DateTime.UtcNow));
        await Task.WhenAll(writeA, writeB);


        Assert.False(mockFileSystem.AllReadersArrivedConcurrently);
        Assert.Equal(2, mockFileSystem.GetAllRecords().Count);
    }

    [Fact]
    public async Task LessNaiveFileStorage_SingleWrite_CompletesWithoutHanging()
    {
        var mockFileSystem = new SimpleMockFileSystem();
        var storage = new BuildingDatabaseInTextFileAsync.LessNaiveFileStorage("test.json", mockFileSystem);

        var writeTask = storage.WriteAsync(new BuildingDatabaseInTextFileAsync.FileRecord(Guid.NewGuid(), "Record-0", "Data-0", DateTime.UtcNow));
        var completed = await Task.WhenAny(writeTask, Task.Delay(TimeSpan.FromSeconds(2)));

        Assert.True(completed == writeTask);
        Assert.Single(mockFileSystem.GetAllRecords());
    }

    [Fact]
    public async Task LessNaiveFileStorage_ManyConcurrentWrites_KeepsAllData()
    {
        const int writerCount = 20;
        var mockFileSystem = new SimpleMockFileSystem();
        var storage = new BuildingDatabaseInTextFileAsync.LessNaiveFileStorage("test.json", mockFileSystem);

        var tasks = Enumerable.Range(0, writerCount)
            .Select(i => storage.WriteAsync(new BuildingDatabaseInTextFileAsync.FileRecord(
                Guid.NewGuid(), $"Record-{i}", $"Data-{i}", DateTime.UtcNow)))
            .ToArray();

        await Task.WhenAll(tasks);

        Assert.Equal(writerCount, mockFileSystem.GetAllRecords().Count);
    }
    
    private class ReadConcurrencyGateFileSystem : BuildingDatabaseInTextFileAsync.IFileSystem
    {
        private string _fileContent = "[]";
        private readonly int _requiredConcurrentReaders;
        private readonly TimeSpan _gateTimeout;
        private int _activeReaders;
        private readonly TaskCompletionSource _requiredConcurrencyReached =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public bool AllReadersArrivedConcurrently { get; private set; }

        public ReadConcurrencyGateFileSystem(int requiredConcurrentReaders, TimeSpan? gateTimeout = null)
        {
            _requiredConcurrentReaders = requiredConcurrentReaders;
            _gateTimeout = gateTimeout ?? TimeSpan.FromMilliseconds(100);
        }

        public async Task<string> ReadAllTextAsync(string path, CancellationToken ct = default)
        {
            var snapshot = _fileContent;
            var activeNow = Interlocked.Increment(ref _activeReaders);
            try
            {
                if (activeNow >= _requiredConcurrentReaders)
                {
                    AllReadersArrivedConcurrently = true;
                    _requiredConcurrencyReached.TrySetResult();
                }

                await Task.WhenAny(_requiredConcurrencyReached.Task, Task.Delay(_gateTimeout, ct));

                return snapshot;
            }
            finally
            {
                Interlocked.Decrement(ref _activeReaders);
            }
        }

        public Task WriteAllTextAsync(string path, string content, CancellationToken ct = default)
        {
            _fileContent = content;
            return Task.CompletedTask;
        }

        public bool FileExists(string path) => true;

        public List<BuildingDatabaseInTextFileAsync.FileRecord> GetAllRecords()
            => JsonSerializer.Deserialize<List<BuildingDatabaseInTextFileAsync.FileRecord>>(_fileContent) ?? [];
    }

    private class SimpleMockFileSystem : BuildingDatabaseInTextFileAsync.IFileSystem
    {
        private string _fileContent = "[]";

        public Task<string> ReadAllTextAsync(string path, CancellationToken ct = default)
            => Task.FromResult(_fileContent);

        public Task WriteAllTextAsync(string path, string content, CancellationToken ct = default)
        {
            _fileContent = content;
            return Task.CompletedTask;
        }

        public bool FileExists(string path) => true;

        public List<BuildingDatabaseInTextFileAsync.FileRecord> GetAllRecords()
            => JsonSerializer.Deserialize<List<BuildingDatabaseInTextFileAsync.FileRecord>>(_fileContent) ?? [];
    }
}