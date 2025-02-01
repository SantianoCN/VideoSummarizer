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
    const value = document.getElementById("wordsCountInput").value;
    const element = document.getElementById("words-count-selector");

    element.value = value;
}

function summBtnClick() {
    const element = document.getElementById("mainInfo");
    element.style.display = 'none';
}

function useAdditionalTask(){
    const isChecked = document.getElementById('useAdditionalTask').checked;
    
    if (isChecked) {
        const element = document.getElementById("additional-area");

        element.style.display = "block";
    } else {
        const element = document.getElementById("additional-area");

        element.style.display = "none";
    }
}
window.onload = loadTheme;