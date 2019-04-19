var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var Blazored;
(function (Blazored) {
    var LocalStorage;
    (function (LocalStorage_1) {
        var LocalStorage = /** @class */ (function () {
            function LocalStorage() {
            }
            LocalStorage.prototype.SetItem = function (key, data) {
                window.localStorage.setItem(key, data);
            };
            LocalStorage.prototype.GetItem = function (key) {
                return window.localStorage.getItem(key);
            };
            LocalStorage.prototype.RemoveItem = function (key) {
                window.localStorage.removeItem(key);
            };
            LocalStorage.prototype.Clear = function () {
                window.localStorage.clear();
            };
            LocalStorage.prototype.Length = function () {
                return window.localStorage.length;
            };
            LocalStorage.prototype.Key = function (index) {
                return window.localStorage.key(index);
            };
            return LocalStorage;
        }());
        function Load() {
            var localStorage = {
                LocalStorage: new LocalStorage()
            };
            if (window['Blazored']) {
                window['Blazored'] = __assign({}, window['Blazored'], localStorage);
            }
            else {
                window['Blazored'] = __assign({}, localStorage);
            }
        }
        LocalStorage_1.Load = Load;
    })(LocalStorage = Blazored.LocalStorage || (Blazored.LocalStorage = {}));
})(Blazored || (Blazored = {}));
Blazored.LocalStorage.Load();
//# sourceMappingURL=blazored.LocalStorage.js.map