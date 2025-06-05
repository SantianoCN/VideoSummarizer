$(document).ready(function () {
    const fileInput = document.getElementById('fileUpload');

    $("#summarizeBtn").click(function () {
        sendRequest();
    });

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
        element.innerHTML = "<i>Загрузка транскрипции..</i>";

        const request = await fetch('/VideoProcessing/upload', {
            body: formData,
            method: "post"
        });

        if  (request.status == 200 ) {
            let data = await request.json();
            onAjaxSuccess(data);
        } else {
            let data = await request.json();
            onAjaxError(request.status, data)
        }
    }

    function onAjaxSuccess(response) {
        console.log('Ответ от сервера:', response);

        const element1 = document.getElementById('source-text-output');

        if (response.sourceText != "") {
            element1.innerHTML = <p>${response.sourceText}</p>;
        } else {
            element1.innerHTML = <i>Выберите новый файл или просто нажмите "Суммаризировать" чтобы отобразить текст из видео/аудио</i>;
        }

        const element = document.getElementById('result-text-output');
        element.classList.remove('animate');

        if (response.summary == "") {
            element.innerHTML = <i>Не удалось получить или отобразить краткое содержание. Повторите попытку, нажав на кнопку "Суммаризировать" или попробуйте позже</i>;
            return;
        }

        let html = '';
        html += <p>;
        element.innerHTML = html;

        render(element, response, html);

        html = </p>;
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
            html += ${data.summary[i]};
            element.innerHTML = html;
            await delay(20);
        }
    }

    function delay(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }
});