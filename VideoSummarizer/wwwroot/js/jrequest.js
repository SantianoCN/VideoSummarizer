$(document).ready(function () {
    const fileInput = document.getElementById('fileUpload');

    $("#summarizeBtn").click(function () {
        sendRequest();
    });

    $("#logInBtn").click(function () {
        sendAuthData();
    });

    $("#intoSignInPageBtn").click(function () {
        toggleForms();
    });

    $("#intoSignUpPageBtn").click(function () {
        toggleForms();
    });

    // $("#summarizeBtn").click(function () {
    //     notification();
    // });

    async function sendRequest() {
        const file = fileInput.files[0];
        const wordsCount = document.getElementById('wordsCountInput').value;
        const showSourceText = document.getElementById('showSourceText').checked;
        const additionalTask = document.getElementById('additionalTask').value;

        if (!file) {
            alert("Выберите файл для загрузки.");
            return;
        }

        const formData = new FormData();
        formData.append('file', file);
        formData.append('wordsCount', wordsCount);
        formData.append('showSourceText', showSourceText);
        formData.append('additionalTask', additionalTask);

        const element = document.getElementById('result-text-output');
        element.classList.add('animate');
        element.innerHTML = `<i>Загрузка транскрипции..</i>`;

        const request = await fetch('/VideoProcessing/Upload', {
            body: formData,
            method: 'POST'
        });

        if  (request.status == 200 ) {
            let data = await request.json();
            onAjaxSuccess(data);
        } else {
            let data = await request.json();
            onAjaxError(request.status, data)
        }
    }

    async function sendAuthData() {
        const login = document.getElementById('login').value;
        const password = document.getElementById('password').value;

        if (!login || !password) {
            alert("Укажите логин и пароль.");
            return;
        };

        const formData = new FormData();
        formData.append('login', "login");
        formData.append('password', "password");

        const request = await fetch('/Authorization/SignIn', {
            body: formData,
            method: 'POST'
        });

        if  (request.status == 200 ) {
            let data = await request.json();
            onAjaxSuccess(data);
        } else {
            if (request.status == 403) {
                alert("Неправильный логин или пароль, попробуйте еще раз.");
            } else {
                let data = await request.json();
                onAjaxError(request.status, data);
            }
        }
    }

    function onAjaxSuccess(response) {
        console.log('Ответ от сервера:', response);

        const element1 = document.getElementById('source-text-output');

        if (response.sourceText != "") {
            element1.innerHTML = `<p>${response.sourceText}</p>`;
        } else {
            element1.innerHTML = `<i>Выберите новый файл или просто нажмите "Суммаризировать" чтобы отобразить текст из видео/аудио</i>`;
        }

        const element = document.getElementById('result-text-output');
        element.classList.remove('animate');

        if (response.summary == "") {
            element.innerHTML = `<i>Не удалось получить или отобразить краткое содержание. Повторите попытку, нажав на кнопку "Суммаризировать" или попробуйте позже</i>`;
            return;
        }

        let html = ``;
        html += `<p>`;
        element.innerHTML = html;

        render(element, response, html);

        html = `</p>`;
        element.innerHTML = html;
    }

    function onAjaxError(status, error) {
        const element = document.getElementById('result-text-output');
        element.classList.remove('animate');
        element.innerHTML = "";

        console.error('Ошибка при отправке запроса:', error);
        if (status >= 500) {
            alert(status + ": Ошибка сервера. Повторите попытку позже"); return;
        }
        switch (status) {
            case 415:
                alert("Ошибка! Неподдерживаемый тип файла");
                break;
            case 413:
                alert("Ошибка! Файл имеет слишком большой размер.");
                break;
        } 
    }

    async function render(element, data, html) {
        for (let i = 0; i < data.summary.length; i++) {
            html += `${data.summary[i]}`;
            element.innerHTML = html;
            await delay(20);
        }
    }

    function delay(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }

    function toggleForms() {
        const leftLogin = document.getElementById('left-login-content');
        const leftRegister = document.getElementById('left-register-content');

        const rightLogin = document.getElementById('right-login-form');
        const rightRegister = document.getElementById('right-register-form');

        if (leftLogin.classList.contains('hidden-form')) {
            leftLogin.classList.remove('hidden-form');
            leftRegister.classList.add('hidden-form');
            rightLogin.classList.remove('hidden-form');
            rightRegister.classList.add('hidden-form');
        } else {
            leftLogin.classList.add('hidden-form');
            leftRegister.classList.remove('hidden-form');
            rightLogin.classList.add('hidden-form');
            rightRegister.classList.remove('hidden-form');
        }
    }

    function notification(text) {
        const element = document.getElementById('notification');
        const textElement = document.getElementById('notification-text');

        textElement.innerText = text;
        element.classList.add('show');

        setTimeout(() => {
            element.classList.remove('show');
        }, 6000);
    }
});