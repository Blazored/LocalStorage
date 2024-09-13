window.setLocalStorageValue = async (key, streamReference) => {
    const arrayBuffer = await streamReference.arrayBuffer();
    const stringValue = new TextDecoder().decode(arrayBuffer);
    localStorage.setItem(key, stringValue);
}
window.getLocalStorageValue = (key) => {
    const value = localStorage.getItem(key);
    const utf8Encoder = new TextEncoder();
    const encodedTextValue = utf8Encoder.encode(value);
    return encodedTextValue;
}