const regex = /^(a+)+$/;

console.time("Case1");
regex.test("aaaaaaaaaa");
console.timeEnd("Case1");

console.time("Case2");
regex.test("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa!");
console.timeEnd("Case2");