window.blazoredLocalStorage = {
    setItem: function (key, data) {
        localStorage.setItem(key, data);
    },
    getItem: function (key) {
        return localStorage.getItem(key);
    },
    removeItem: function (key) {
        localStorage.removeItem(key);
    },
    clear: function () {
        localStorage.clear();
    },
    length: function () {
        return localStorage.length;
    },
    key: function (index) {
        return localStorage.key(index);
    }
};