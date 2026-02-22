document.addEventListener('DOMContentLoaded', () => {
        const dropZone = document.getElementById('drop-zone');
        const fileInput = document.getElementById('file-input');
        const previewImg = document.getElementById('preview-img');
        const uploadInfo = document.getElementById('upload-info');
        const resetBtn = document.getElementById('reset-btn');

        // Klick auf Fläche öffnet Dateidialog
        dropZone.addEventListener('click', () => fileInput.click());

        // Drag & Drop Events
        ['dragenter', 'dragover'].forEach(e => {
            dropZone.addEventListener(e, (ev) => {
                ev.preventDefault();
                dropZone.classList.add('drag-over');
            });
        });

        ['dragleave', 'drop'].forEach(e => {
            dropZone.addEventListener(e, (ev) => {
                ev.preventDefault();
                dropZone.classList.remove('drag-over');
            });
        });

        dropZone.addEventListener('drop', (e) => {
            const files = e.dataTransfer.files;
            if (files.length) handleFile(files[0]);
        });

        fileInput.addEventListener('change', (e) => {
            if (e.target.files.length) handleFile(e.target.files[0]);
        });

        function handleFile(file) {
            if (!file.type.startsWith('image/')) return;

            const reader = new FileReader();
            reader.onload = (e) => {
                previewImg.src = e.target.result;
                previewImg.style.display = 'block'; // Bild anzeigen
                uploadInfo.classList.add('hidden'); // Text verstecken
                resetBtn.style.display = 'block';   // Löschen-Button zeigen
            };
            reader.readAsDataURL(file);
        }

        // Reset Funktion
        resetBtn.addEventListener('click', () => {
            previewImg.src = '';
            previewImg.style.display = 'none';
            uploadInfo.classList.remove('hidden');
            resetBtn.style.display = 'none';
            fileInput.value = '';
        });
    });