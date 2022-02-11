function scripts() {
    let uploadFilesElement = document.getElementById('files');
    let descriptionElement = document.getElementById('files-hidden');

    uploadFilesElement.addEventListener('change', () => {
        let uploadFiles = uploadFilesElement.files;
        const dT = new DataTransfer();

        let visualiseUploadFilesElement = document.getElementById('visualise-upload-files');
        visualiseUploadFilesElement.hidden = false;

        for (const file of uploadFiles) {
            dT.items.add(file);
            let divElement = document.createElement('div');
            divElement.textContent = file.name;
            divElement.style = 'display:inline';
            divElement.classList.add('form-group');
            divElement.id = 'file-visualise';
            divElement.addEventListener('click', () => {
                divElement.remove();
                dT.items.remove(file);
                descriptionElement.files = dT.files;
            });
            visualiseUploadFilesElement.appendChild(divElement);
        }
        for (const file of descriptionElement.files) {
            dT.items.add(file);
        }
        descriptionElement.files = dT.files;
    });

    let files = document.querySelectorAll('#file-visualise');
    for (const file of files) {
        file.addEventListener('click', () => {
            const dataTransfer = new DataTransfer();
            for (const file of descriptionElement.files) {
                dataTransfer.items.add(file);
            }
            console.log(dataTransfer.files);
        })
    }
}

scripts();