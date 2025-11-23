
window.addEventListener('load', function () {
    const updateAuthPlaceholder = () => {
        const input = document.querySelector('.auth-container input[type="text"]');
        if (input && !input.hasAttribute('data-enhanced')) {
            input.setAttribute('placeholder', 'Bearer seu_token_jwt');
            input.setAttribute('data-enhanced', 'true');
        }
    };

    const addEndpointCount = () => {
        const title = document.querySelector('.info .title');
        if (title && !document.querySelector('.endpoint-count')) {
            const count = document.querySelectorAll('.opblock').length;
            const badge = document.createElement('span');
            badge.textContent = ` (${count} endpoints)`;
            badge.style.fontSize = '0.9rem';
            badge.style.color = '#6b7280';
            title.appendChild(badge);
        }
    };

    setTimeout(() => {
        updateAuthPlaceholder();
        addEndpointCount();
    }, 500);
});