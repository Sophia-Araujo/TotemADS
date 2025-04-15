
    const input = document.getElementById('input');
    const row1 = document.getElementById('row1');
    const row2 = document.getElementById('row2');
    const row3 = document.getElementById('row3');
    const row4 = document.getElementById('row4');
    const row5 = document.getElementById('row5');

    const keysRow1 = ['1','2','3','4','5','6','7','8','9','0'];
    const keysRow2 = ['Q','W','E','R','T','Y','U','I','O','P'];
    const keysRow3 = ['A','S','D','F','G','H','J','K','L'];
    const keysRow4 = ['Z','X','C','V','B','N','M'];

    let isUpper = true;

    function createKey(char) {
      const key = document.createElement('button');
      key.classList.add('key');
      key.textContent = isUpper ? char.toUpperCase() : char.toLowerCase();
      key.onclick = () => input.value += key.textContent;
      return key;
    }

    function renderKeyboard() {
      row1.innerHTML = '';
      row2.innerHTML = '';
      row3.innerHTML = '';
      row4.innerHTML = '';

      keysRow1.forEach(char => row1.appendChild(createKey(char)));
      keysRow2.forEach(char => row2.appendChild(createKey(char)));
      keysRow3.forEach(char => row3.appendChild(createKey(char)));

      const shiftKey = document.createElement('button');
      shiftKey.classList.add('key', 'shift');
      if (isUpper) shiftKey.classList.add('active');
      shiftKey.innerHTML = '⇧';
      shiftKey.onclick = () => {
        isUpper = !isUpper;
        renderKeyboard();
      };

      const deleteKey = document.createElement('button');
      deleteKey.classList.add('key', 'delete');
      deleteKey.innerHTML = '⌫';
      deleteKey.onclick = () => input.value = input.value.slice(0, -1);

      row4.appendChild(shiftKey);
      keysRow4.forEach(char => row4.appendChild(createKey(char)));
      row4.appendChild(deleteKey);

      const space = document.createElement('button');
      space.classList.add('key', 'space');
      space.textContent = 'espaço';
      space.onclick = () => input.value += ' ';

      row5.innerHTML = '';
      row5.appendChild(space);
    }

    renderKeyboard();