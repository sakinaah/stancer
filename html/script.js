// XStance Menu JavaScript
let currentStanceData = {
    frontTrackWidth: 0.0,
    rearTrackWidth: 0.0,
    frontCamber: 0.0,
    rearCamber: 0.0
};

// DOM Elements
const menu = document.getElementById('menu');
const closeBtn = document.getElementById('closeBtn');
const resetBtn = document.getElementById('resetBtn');
const saveBtn = document.getElementById('saveBtn');

// Sliders
const frontTrackWidthSlider = document.getElementById('frontTrackWidth');
const rearTrackWidthSlider = document.getElementById('rearTrackWidth');
const frontCamberSlider = document.getElementById('frontCamber');
const rearCamberSlider = document.getElementById('rearCamber');

// Value displays
const frontTrackWidthValue = document.getElementById('frontTrackWidthValue');
const rearTrackWidthValue = document.getElementById('rearTrackWidthValue');
const frontCamberValue = document.getElementById('frontCamberValue');
const rearCamberValue = document.getElementById('rearCamberValue');

// Initialize event listeners
document.addEventListener('DOMContentLoaded', function() {
    initializeSliders();
    initializeButtons();
    initializeNUIListeners();
});

function initializeSliders() {
    // Front Track Width
    frontTrackWidthSlider.addEventListener('input', function() {
        const value = parseFloat(this.value);
        frontTrackWidthValue.textContent = value.toFixed(2);
        currentStanceData.frontTrackWidth = value;
        sendStanceUpdate();
    });

    // Rear Track Width
    rearTrackWidthSlider.addEventListener('input', function() {
        const value = parseFloat(this.value);
        rearTrackWidthValue.textContent = value.toFixed(2);
        currentStanceData.rearTrackWidth = value;
        sendStanceUpdate();
    });

    // Front Camber
    frontCamberSlider.addEventListener('input', function() {
        const value = parseFloat(this.value);
        frontCamberValue.textContent = value.toFixed(2);
        currentStanceData.frontCamber = value;
        sendStanceUpdate();
    });

    // Rear Camber
    rearCamberSlider.addEventListener('input', function() {
        const value = parseFloat(this.value);
        rearCamberValue.textContent = value.toFixed(2);
        currentStanceData.rearCamber = value;
        sendStanceUpdate();
    });
}

function initializeButtons() {
    // Close button
    closeBtn.addEventListener('click', function() {
        closeMenu();
    });

    // Reset button
    resetBtn.addEventListener('click', function() {
        resetAllValues();
        fetch('http://xstance/resetStance', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({})
        }).catch(() => {});
    });

    // Save button
    saveBtn.addEventListener('click', function() {
        // TODO: Implement preset saving functionality
        showNotification('Preset saved!');
    });
}

function initializeNUIListeners() {
    // Listen for NUI messages from Lua
    window.addEventListener('message', function(event) {
        const data = event.data;
        
        switch(data.action) {
            case 'openMenu':
                openMenu(data.stanceData || {});
                break;
            case 'closeMenu':
                closeMenu();
                break;
            case 'updateValues':
                updateSliderValues(data.stanceData || {});
                break;
        }
    });

    // Close menu on ESC key
    document.addEventListener('keydown', function(event) {
        if (event.key === 'Escape') {
            closeMenu();
        }
    });
}

function openMenu(stanceData) {
    currentStanceData = { ...stanceData };
    updateSliderValues(stanceData);
    menu.classList.remove('hidden');
}

function closeMenu() {
    menu.classList.add('hidden');
    fetch('http://xstance/closeMenu', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({})
    }).catch(() => {});
}

function updateSliderValues(stanceData) {
    // Update sliders and value displays
    frontTrackWidthSlider.value = stanceData.frontTrackWidth || 0.0;
    frontTrackWidthValue.textContent = (stanceData.frontTrackWidth || 0.0).toFixed(2);
    
    rearTrackWidthSlider.value = stanceData.rearTrackWidth || 0.0;
    rearTrackWidthValue.textContent = (stanceData.rearTrackWidth || 0.0).toFixed(2);
    
    frontCamberSlider.value = stanceData.frontCamber || 0.0;
    frontCamberValue.textContent = (stanceData.frontCamber || 0.0).toFixed(2);
    
    rearCamberSlider.value = stanceData.rearCamber || 0.0;
    rearCamberValue.textContent = (stanceData.rearCamber || 0.0).toFixed(2);
    
    currentStanceData = { ...stanceData };
}

function resetAllValues() {
    const resetData = {
        frontTrackWidth: 0.0,
        rearTrackWidth: 0.0,
        frontCamber: 0.0,
        rearCamber: 0.0
    };
    
    updateSliderValues(resetData);
}

function sendStanceUpdate() {
    fetch('http://xstance/updateStance', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            values: currentStanceData
        })
    }).catch(() => {});
}

function showNotification(message) {
    // Create a simple notification
    const notification = document.createElement('div');
    notification.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        background: #00b894;
        color: white;
        padding: 10px 20px;
        border-radius: 8px;
        font-weight: 600;
        z-index: 1000;
        animation: slideIn 0.3s ease;
    `;
    notification.textContent = message;
    
    document.body.appendChild(notification);
    
    setTimeout(() => {
        notification.style.animation = 'slideOut 0.3s ease';
        setTimeout(() => {
            document.body.removeChild(notification);
        }, 300);
    }, 2000);
}

// Add CSS animations
const style = document.createElement('style');
style.textContent = `
    @keyframes slideIn {
        from { transform: translateX(100%); opacity: 0; }
        to { transform: translateX(0); opacity: 1; }
    }
    
    @keyframes slideOut {
        from { transform: translateX(0); opacity: 1; }
        to { transform: translateX(100%); opacity: 0; }
    }
`;
document.head.appendChild(style);