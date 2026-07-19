import pako from 'pako';
import AsyncStorage from '@react-native-async-storage/async-storage';

const buildSyncRequest = async (): Promise<Uint8Array> => {
  const keys = await AsyncStorage.getAllKeys();
  const pairs = await AsyncStorage.multiGet(keys);
  
  const data = Object.fromEntries(
    pairs
      .filter(([_, v]) => v !== null)
      .map(([k, v]) => [k, JSON.parse(v!)])
  );

  const payload = {
    exportDate: new Date().toISOString(),
    appVersion: '2.0.0',
    platform: Platform.OS,
    data
  };

  const json = JSON.stringify(payload);
  return pako.gzip(json);
};

const sync = async () => {
  const compressed = await buildSyncRequest();
  
  await fetch(`${API_URL}/sync`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Content-Encoding': 'gzip',
    },
    body: compressed,
  });
};
