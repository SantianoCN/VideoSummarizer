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

        const request = await fetch('/VideoProcessing/upload', {
            body: formData,
            method: "post"
        });
        let data = await request.json();

        onAjaxSuccess(data);
    }

    function onAjaxSuccess(response) {
        console.log('Ответ от сервера:', response);

        const element1 = document.getElementById('source-text-output');

        if (response.sourceText != "") {
            element1.innerHTML = `<p>${response.sourceText}</p>`;
        }

        const element = document.getElementById('result-text-output');

        let html = '';
        html += `<p><strong>`;
        element.innerHTML = html;

        render(element, response, html);

        html = `</strong></p>`;
        element.innerHTML = html;
    }

    function onAjaxError(xhr, status, error) {
        console.error('Ошибка при отправке запроса:', error);
        alert('Произошла ошибка при отправке запроса. Проверьте консоль для подробностей.');
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
});