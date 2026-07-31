// compress.js
const pako = require('pako');
const fs = require('fs');

const filePath = process.argv[2];
if (!filePath) {
  console.error('Usage: node compress.js <file.json>');
  process.exit(1);
}

const json = fs.readFileSync(filePath, 'utf-8');
const compressed = pako.gzip(json);
const outPath = filePath.replace('.json', '.json.gz');
fs.writeFileSync(outPath, compressed);
console.log(`${filePath} → ${outPath} (${json.length} → ${compressed.length} bytes)`);
