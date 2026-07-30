using System.Text.Json;

namespace Research.Shared.Articles;

public class BuildingDatabaseInTextFileAsync
{
    public class LessNaiveFileStorage : IFileStorage<FileRecord>
    {
        private readonly string _filePath;
        private readonly IFileSystem _fileSystem;
        private readonly SemaphoreSlim _semaphore;

        public LessNaiveFileStorage(string filePath, IFileSystem? fileSystem = null)
        {
            _semaphore = new SemaphoreSlim(1, 1);
            _filePath = filePath;
            _fileSystem = fileSystem ?? new DefaultFileSystem();

            if (!_fileSystem.FileExists(_filePath))
                _fileSystem.WriteAllTextAsync(_filePath, "[]").Wait();
        }

        public async Task WriteAsync(FileRecord record, CancellationToken ct = default)
        {
            await _semaphore.WaitAsync(ct);
            
            var json = await _fileSystem.ReadAllTextAsync(_filePath, ct);
            var records = JsonSerializer.Deserialize<List<FileRecord>>(json) ?? [];

            records.Add(record);

            await _fileSystem.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(records), ct);
            
            _semaphore.Release();
        }
    }
    
    public class NaiveFileStorage : IFileStorage<FileRecord>
    {
        private readonly string _filePath;
        private readonly IFileSystem _fileSystem;

        public NaiveFileStorage(string filePath, IFileSystem? fileSystem = null)
        {
            _filePath = filePath;
            _fileSystem = fileSystem ?? new DefaultFileSystem();

            if (!_fileSystem.FileExists(_filePath))
                _fileSystem.WriteAllTextAsync(_filePath, "[]").Wait();
        }

        public async Task WriteAsync(FileRecord record, CancellationToken ct = default)
        {
            var json = await _fileSystem.ReadAllTextAsync(_filePath, ct);
            var records = JsonSerializer.Deserialize<List<FileRecord>>(json) ?? [];

            records.Add(record);

            await _fileSystem.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(records), ct);
        }
    }

    private class DefaultFileSystem : IFileSystem
    {
        public async Task<string> ReadAllTextAsync(string path, CancellationToken ct = default)
            => await File.ReadAllTextAsync(path, ct);

        public async Task WriteAllTextAsync(string path, string content, CancellationToken ct = default)
            => await File.WriteAllTextAsync(path, content, ct);

        public bool FileExists(string path) => File.Exists(path);
    }
    
    public interface IFileSystem
    {
        Task<string> ReadAllTextAsync(string path, CancellationToken ct = default);
        Task WriteAllTextAsync(string path, string content, CancellationToken ct = default);
        bool FileExists(string path);
    }
    
    public interface IFileStorage<T>
    {
        Task WriteAsync(T record, CancellationToken ct = default);
    }
    
    public record FileRecord(
        Guid Id,
        string Name,
        string Payload,
        DateTime CreatedAt
    );
}