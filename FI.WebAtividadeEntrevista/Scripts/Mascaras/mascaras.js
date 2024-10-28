function maskCpf(event) {
    const pattern = "000.000.000-00";
    customMask(event, pattern);
}

function maskCep(event) {
    const pattern = "00000-000";
    customMask(event, pattern);
}

function maskTelefone(event) {
    const pattern = "(00) 00000-0000";
    customMask(event, pattern);
}

// X: Letra e numero
// 0: Somente numeros
// A: Somente letras
function customMask(event, maskPattern) {
    const aux = event.target;
    const value = aux.value;
    const keyPressed = event.keyCode;
    const keysReturn = (keyPressed == 17 || keyPressed == 16 || event.ctrlKey || event.shiftKey);

    if (keysReturn && keyPressed != 86) return;

    let cursorPosition = aux.selectionStart; 
    let cleanedValue = value.replace(/[^a-zA-Z0-9]/g, "");
    let maskedValue = "";
    let patternIndex = 0;
    let valueIndex = 0;

    while (patternIndex < maskPattern.length && valueIndex < cleanedValue.length) {
        let patternChar = maskPattern[patternIndex];
        let currentChar = cleanedValue[valueIndex];

        if (patternChar === 'X') {
            if (/[a-zA-Z0-9]/.test(currentChar)) {
                maskedValue += currentChar;
            }

            valueIndex++;
        } else if (patternChar === 'A') {
            if (/[a-zA-Z]/.test(currentChar)) {
                maskedValue += currentChar;
            } 

            valueIndex++;
        } else if (patternChar === '0') {
            if (/[0-9]/.test(currentChar)) {
                maskedValue += currentChar;
            }

            valueIndex++;
        } else {

            if (currentChar !== null) {
                maskedValue += patternChar;
            }

            if (keyPressed != 8 && cursorPosition >= cleanedValue.length) {
                cursorPosition++;
            }
        }

        patternIndex++;
    }

    
    const mask = maskPattern.replace(/[0AX]/gi, "");
    if (mask != maskedValue) {
        aux.value = maskedValue;
    }
    else {
        aux.value = "";
    }

    //37 Left, 38 Up, 39 Right, 40 Down
    const arrowsKey = [37, 38, 39, 40];

    if (!arrowsKey.includes(keyPressed)) {
        aux.setSelectionRange(cursorPosition, cursorPosition);
    }
}

function removeCustomMask(value, maskPattern) {
    const maskComponents = maskPattern.replace(/[0AX]/gi, "");
    let unmaskedValue = "";

    for (let char of value) {
        if (!maskComponents.includes(char)) {
            unmaskedValue += char;
        }
    }

    return unmaskedValue;
}