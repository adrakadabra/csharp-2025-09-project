window.setupQrListener = (dotNetHelper) => {
    let buffer = "";
    let timeoutId = null;

    const resetBuffer = () => {
        buffer = "";
        if (timeoutId) clearTimeout(timeoutId);
    };

    const processBuffer = () => {
        const match = buffer.match(/#AU-[^\s]+/);
        if (match) {
            const fullCode = match[0];
            console.log("Отправка в C#:", fullCode);
            dotNetHelper.invokeMethodAsync('OnQrScanned', fullCode);
        }
        resetBuffer();
    };

    // Обработка символов (печатные символы)
    document.addEventListener('keypress', (e) => {
        // Игнорируем, если это управляющая клавиша (Enter, Backspace и т.д.)
        if (e.key.length !== 1) return;

        // Сбрасываем таймер при каждом новом символе
        if (timeoutId) clearTimeout(timeoutId);
        timeoutId = setTimeout(() => {
            console.log("Таймаут: очистка буфера");
            resetBuffer();
        }, 100); // 100 мс без ввода — сброс

        buffer += e.key;
        console.log("Буфер:", buffer);

        // Если в буфере уже есть пробел или Enter, можно сразу обработать
        if (buffer.includes(' ') || buffer.includes('\n')) {
            processBuffer();
        }
    });

    // Обработка завершающих клавиш (Enter, пробел)
    document.addEventListener('keydown', (e) => {
        if (e.key === " " || e.key === "Enter") {
            console.log("Завершающий символ:", e.key);
            // Если пробел или Enter уже попали в буфер через keypress,
            // но мы обрабатываем здесь на случай, если keypress не сработал.
            if (buffer.length > 0) {
                processBuffer();
            }
            e.preventDefault(); // чтобы пробел не прокручивал страницу
        }

        // Очистка при слишком длинном буфере
        if (buffer.length > 50) resetBuffer();
    });
};