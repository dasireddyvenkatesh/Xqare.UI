// Replace the entire file with this
self.addEventListener('install', event => event.waitUntil(onInstall(event)));
self.addEventListener('activate', event => event.waitUntil(onActivate(event)));
self.addEventListener('fetch', event => event.respondWith(onFetch(event)));

async function onInstall(event) {
    self.skipWaiting();
}

async function onActivate(event) {
    // Delete ALL old caches on every deploy
    const cacheKeys = await caches.keys();
    await Promise.all(cacheKeys.map(key => caches.delete(key)));
    await clients.claim();
}

async function onFetch(event) {
    // Never cache — always go to network
    // This is the nuclear option that guarantees fresh content
    return fetch(event.request, { cache: 'no-store' });
}