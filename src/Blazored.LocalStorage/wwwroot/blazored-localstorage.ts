namespace Blazored.LocalStorage {
    class LocalStorage {
        public SetItem(key: string, data: string): void {
            window.localStorage.setItem(key, data);
        }

        public GetItem(key: string): string {
            return window.localStorage.getItem(key);
        }

        public RemoveItem(key: string): void {
            window.localStorage.removeItem(key);
        }

        public Clear(): void {
            window.localStorage.clear();
        }

        public Length(): number {
            return window.localStorage.length;
        }

        public Key(index: number): string {
            return window.localStorage.key(index);
        }
    }

    export function Load(): void {
        const localStorage:
        any = {
            LocalStorage: new LocalStorage()
        };

        if (window['Blazored']) {
            window['Blazored'] = {
                ...window['Blazored'],
                ...localStorage
            }
        } else {
            window['Blazored'] = {
                ...localStorage
            }
        }
    }
}

Blazored.LocalStorage.Load();
