function setTheme(themeName) {
    document.documentElement.setAttribute('data-theme', themeName);
    localStorage.setItem('theme', themeName);
}

function loadTheme() {
    const savedTheme = localStorage.getItem('theme') || 'default';
    setTheme(savedTheme);
}

function rangeShowValue(){
    const element = document.getElementById("wordsCountInput");
    const value = document.getElementById("words-count-selector").value;
    
    element.value = value;
}

function rangeHandChange(){
    const value = document.getElementById('wordsCountInput').value;
    const element = document.getElementById('words-count-selector');

    element.value = value;
}

function summBtnClick() {
    const element = document.getElementById('mainInfo');
    element.style.display = 'none';
}

function showSourceText() {
    const showSourceText = document.getElementById('showSourceText').checked;
    
    if (showSourceText) {
        const element = document.getElementById('source-text-area');

        element.style.display = "flex";
    } else {
        const element = document.getElementById('source-text-area');

        element.style.display = "none";
    }
}

function useAdditionalTask(){
    const isChecked = document.getElementById('useAdditionalTask').checked;
    
    if (isChecked) {
        const element = document.getElementById('additional-area');

        element.style.display = "block";
    } else {
        const element = document.getElementById('additional-area');

        element.style.display = "none";
    }
}

function copySourceText() {
    const element = document.getElementById('source-text-output');
    const text = element.textContent;
    
    navigator.clipboard.writeText(text);
    notification("Текст скопирован в буфер обмена");
}

function copyResultText() {
    const element = document.getElementById('result-text-output');
    const text = element.textContent;
    
    navigator.clipboard.writeText(text);
    notification("Текст скопирован в буфер обмена");
}

function notification(text) {
    const element = document.getElementById('notification-window');
    const textElement = document.getElementById('notification-text');
    
    textElement.innerText = text;
    element.classList.remove('hide');
    element.classList.remove('animate-hide');
    
    element.classList.add('show');
    element.classList.add('animate-show');

    setTimeout(() => {
        element.classList.remove('show')
        element.classList.remove('animate-show');

        element.classList.add('hide');
        element.classList.add('animate-hide');
    }, 6000);
}

window.onload = loadTheme;