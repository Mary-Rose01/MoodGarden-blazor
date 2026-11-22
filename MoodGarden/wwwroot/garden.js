window.garden = {
    getRect: (el) => {
        const r = el.getBoundingClientRect();
        return { left: r.left, top: r.top, width: r.width, height: r.height };
    },
    load: (key) => {
        try { return localStorage.getItem(key); } catch { return null; }
    },
    save: (key, value) => {
        try { localStorage.setItem(key, value); } catch { }
    },
    hour: () => new Date().getHours(),
    setTheme: (isDark) => {
        document.documentElement.dataset.theme = isDark ? 'dark' : '';
    },
    getClientId: () => {
        const key = 'cat-mood-garden:client-id';
        try {
            let id = localStorage.getItem(key);
            if (!id) {
                id = (crypto && crypto.randomUUID) ? crypto.randomUUID() : String(Date.now()) + Math.random().toString(16).slice(2);
                localStorage.setItem(key, id);
            }
            return id;
        } catch {
            return 'anonymous-gardener';
        }
    }
};
