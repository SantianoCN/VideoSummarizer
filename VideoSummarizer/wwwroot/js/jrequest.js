$(document).ready(function () {
    const fileInput = document.getElementById('fileInput');



    $("#summarizeBtn").click(function () {
        const file = fileInput.files[0];

        if (!file) {
            alert("Выберите файл для загрузки.");
            return;
        }


        const formData = new FormData();
        formData.append('file', file);

        const xhr = new XMLHttpRequest();

        xhr.open('POST', '/VideoProcessing/upload');    // адрес контроллера
        xhr.upload.onprogress = (event) => {
            if (event.lengthComputable) {
                const percentComplete = (event.loaded / event.total) * 100;
                console.log(`Загрузка: ${percentComplete.toFixed(2)}%`);
            }
        }

        async function render(element, data, html) {
            for (let i = 0; i < data.text.length; i++) {
                html += `${data.text[i]}`;
                element.innerHTML = html;
                await delay(20);
            }
        }

        function delay(ms) {
            return new Promise(resolve => setTimeout(resolve, ms));
        }

        xhr.onloadstart = () => {
            const progressbar = document.getElementById('progressbar');

            let i = 0;
            function update() {
                if (i <= 100) {
                    progressbar.setAttribute('aria-valuenow', `${i}`);
                    progressbar.style.width = `${i}%`;

                    i++;
                    setTimeout(update, 50)
                }
            }
            update();
        }

        xhr.onload = () => {
            const progressbar = document.getElementById('progressbar');
            progressbar.setAttribute('aria-valuenow', `${0}`);
            progressbar.style.width = `${0}%`;

            if (xhr.status >= 200 && xhr.status < 300) {
                const element = document.getElementById('output');
                let data = JSON.parse(xhr.response);

                let html = '';
                element.innerHTML -= `Идет загрузка транскрипции..`;
                html += `<p><strong>`;
                element.innerHTML = html;

                render(element, data, html);

                html = `</strong></p>`;
                element.innerHTML = html;

                console.log('Ответ получен.');
            } else {
                const element = document.getElementById('output');

                let data = {
                    message: ""
                };

                let errorData = JSON.parse(xhr.response);

                element.innerHTML = `<h2><strong style="color: darkred">Ошибка: ${errorData.message}</strong></h2>`;
            }
        }

        xhr.onerror = () => {
            console.error('Ошибка сервера.');
            alert('500 Ошибка сервера.');
        }

        xhr.send(formData);

        // var data = {
        //     WordsCount: $("#wordsCount").val(),
        //     GetFullText: $("#getFullText").is(":checked")
        // };

        // $.ajax({
        //     url: '/VideoProcessing/upload',
        //     type: 'POST',
        //     data: JSON.stringify(data),
        //     contentType: "application/json",
        //     dataType: "json",
        //     success: onAjaxSuccess
        // });
    });
});