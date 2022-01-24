export function loadCss(cssUrl) {
    return new Promise((resolve, reject) => {
        var link = document.createElement('link');

        link.rel = 'stylesheet';
        link.href = cssUrl;
        document.head.appendChild(link);
        link.onload = function () {
            resolve();
            console.log('CSS has loaded!');
        };
        link.onerror = () => reject(new Error(`Css load error for ${cssUrl}`));
    });
}