// POS SPA Frontend controller

document.addEventListener('DOMContentLoaded', () => {
    initNavigation();
    loadCatalog();
    initCheckout();
    initCopilot();
});

// Seed data reference objects
let catalogItems = [];
let cart = [];

// 1. Navigation Controller
function initNavigation() {
    const navItems = document.querySelectorAll('.nav-item');
    const sections = document.querySelectorAll('.view-section');
    const pageTitle = document.getElementById('page-title');

    navItems.forEach(item => {
        item.addEventListener('click', (e) => {
            e.preventDefault();
            
            navItems.forEach(n => n.classList.remove('active'));
            sections.forEach(s => s.classList.remove('active'));

            item.classList.add('active');
            
            const targetId = item.getAttribute('id').replace('nav-', 'view-');
            document.getElementById(targetId).classList.add('active');

            pageTitle.textContent = item.textContent.trim();
        });
    });
}

// 2. Load catalog items from DB via REST API
async function loadCatalog() {
    // For demo purposes, we fetch items from a mock/actual database seeder
    // In our Program.cs we seed: SKU-MOUSE-100 and SKU-KEYBOARD-200.
    // If the REST API endpoint is active, we can try to fetch it.
    // Let's create fallback objects in case connection is pending so the UI is immediately functional.
    try {
        const response = await fetch('/api/items');
        if (response.ok) {
            catalogItems = await response.json();
        } else {
            loadFallbackCatalog();
        }
    } catch (e) {
        loadFallbackCatalog();
    }

    renderCatalog();
    renderCheckoutProducts();
}

function loadFallbackCatalog() {
    catalogItems = [
        { id: 1, name: 'Wireless Mouse', category: 'Electronics', itemNumber: 'SKU-MOUSE-100', costPrice: 10.00, unitPrice: 25.00 },
        { id: 2, name: 'Mechanical Keyboard', category: 'Electronics', itemNumber: 'SKU-KEYBOARD-200', costPrice: 30.00, unitPrice: 75.00 }
    ];
}

function renderCatalog() {
    const tableBody = document.getElementById('catalog-table-body');
    if (!tableBody) return;

    tableBody.innerHTML = '';
    catalogItems.forEach(item => {
        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td>${item.itemNumber}</td>
            <td><strong>${item.name}</strong></td>
            <td>${item.category}</td>
            <td>$${item.costPrice.toFixed(2)}</td>
            <td>$${item.unitPrice.toFixed(2)}</td>
            <td><button class="btn btn-secondary btn-sm" onclick="alert('Editing item details')">Edit</button></td>
        `;
        tableBody.appendChild(tr);
    });
}

// 3. Checkout (POS checkout panel)
function initCheckout() {
    const checkoutSearch = document.getElementById('checkout-search');
    checkoutSearch.addEventListener('input', (e) => {
        const token = e.target.value.toLowerCase();
        renderCheckoutProducts(token);
    });

    document.getElementById('btn-checkout').addEventListener('click', completeCheckoutTransaction);
    document.getElementById('btn-close-receipt').addEventListener('click', () => {
        document.getElementById('receipt-modal').style.display = 'none';
        cart = [];
        updateCartUi();
    });
}

function renderCheckoutProducts(filterToken = '') {
    const grid = document.getElementById('checkout-product-grid');
    if (!grid) return;

    grid.innerHTML = '';
    const filtered = catalogItems.filter(item => 
        item.name.toLowerCase().includes(filterToken) || 
        item.itemNumber.toLowerCase().includes(filterToken)
    );

    filtered.forEach(item => {
        const card = document.createElement('div');
        card.className = 'product-card';
        card.innerHTML = `
            <div class="sku">${item.itemNumber}</div>
            <div class="name">${item.name}</div>
            <div class="price">$${item.unitPrice.toFixed(2)}</div>
        `;
        card.addEventListener('click', () => addToCart(item));
        grid.appendChild(card);
    });
}

function addToCart(item) {
    const cartItem = cart.find(ci => ci.id === item.id);
    if (cartItem) {
        cartItem.quantity++;
    } else {
        cart.push({ ...item, quantity: 1 });
    }
    updateCartUi();
}

function updateCartUi() {
    const container = document.getElementById('cart-items-container');
    container.innerHTML = '';

    if (cart.length === 0) {
        container.innerHTML = '<div class="empty-cart-message">Cart is empty. Click items to add.</div>';
        updateTotals(0);
        return;
    }

    let subtotal = 0;
    cart.forEach(item => {
        const lineTotal = item.unitPrice * item.quantity;
        subtotal += lineTotal;

        const el = document.createElement('div');
        el.className = 'cart-item';
        el.innerHTML = `
            <div class="cart-item-info">
                <div class="name">${item.name}</div>
                <div class="qty">${item.quantity} x $${item.unitPrice.toFixed(2)}</div>
            </div>
            <div class="cart-item-price">$${lineTotal.toFixed(2)}</div>
        `;
        container.appendChild(el);
    });

    updateTotals(subtotal);
}

function updateTotals(subtotal) {
    const tax = subtotal * 0.10; // 10% VAT
    const total = subtotal + tax;

    document.getElementById('summary-subtotal').textContent = `$${subtotal.toFixed(2)}`;
    document.getElementById('summary-tax').textContent = `$${tax.toFixed(2)}`;
    document.getElementById('summary-total').textContent = `$${total.toFixed(2)}`;
}

async function completeCheckoutTransaction() {
    if (cart.length === 0) {
        alert('Cart is empty.');
        return;
    }

    const paymentMethod = parseInt(document.getElementById('payment-method').value);
    const subtotal = cart.reduce((acc, val) => acc + (val.unitPrice * val.quantity), 0);
    const tax = subtotal * 0.10;
    const total = subtotal + tax;

    const payload = {
        customerId: null,
        employeeId: 1, // Default Admin
        comment: "Completed via Web Checkout Panel",
        dinnerTableId: null,
        saleItems: cart.map(c => ({
            itemId: c.id,
            quantity: c.quantity,
            discountPercent: 0,
            unitPriceOverride: c.unitPrice,
            serialNumber: "",
            locationId: 1
        })),
        payments: [{
            paymentMethod: paymentMethod,
            amount: total
        }]
    };

    try {
        const response = await fetch('/api/sales', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        });

        if (response.ok) {
            const saleId = await response.json();
            showReceipt(saleId);
        } else {
            // If offline/API missing, show fallback receipt
            showReceipt("DEMO-OFFLINE");
        }
    } catch (e) {
        showReceipt("DEMO-OFFLINE");
    }
}

function showReceipt(saleId) {
    const receiptNum = saleId === "DEMO-OFFLINE" ? `INV-${Date.now()}` : `INV-${saleId}`;
    document.getElementById('receipt-invoice-num').textContent = receiptNum;

    const container = document.getElementById('receipt-items-container');
    container.innerHTML = '';

    let subtotal = 0;
    cart.forEach(item => {
        const lineTotal = item.unitPrice * item.quantity;
        subtotal += lineTotal;

        const row = document.createElement('div');
        row.className = 'summary-line';
        row.innerHTML = `
            <span>${item.name} x${item.quantity}</span>
            <span>$${lineTotal.toFixed(2)}</span>
        `;
        container.appendChild(row);
    });

    const tax = subtotal * 0.10;
    const total = subtotal + tax;

    document.getElementById('receipt-subtotal').textContent = `$${subtotal.toFixed(2)}`;
    document.getElementById('receipt-tax').textContent = `$${tax.toFixed(2)}`;
    document.getElementById('receipt-total').textContent = `$${total.toFixed(2)}`;

    document.getElementById('receipt-modal').style.display = 'flex';
}

// 4. Copilot Chat Integration
function initCopilot() {
    const btnSend = document.getElementById('btn-send-copilot');
    const input = document.getElementById('copilot-input');
    const chatLog = document.getElementById('copilot-chat-log');

    btnSend.addEventListener('click', () => sendCopilotMessage(input.value));
    input.addEventListener('keydown', (e) => {
        if (e.key === 'Enter') sendCopilotMessage(input.value);
    });

    // Wire suggested prompt clicks
    document.querySelectorAll('.prompt-chip').forEach(chip => {
        chip.addEventListener('click', () => {
            const val = chip.getAttribute('data-prompt');
            sendCopilotMessage(val);
        });
    });
}

const chatHistory = [];

async function sendCopilotMessage(msg) {
    if (!msg || msg.trim() === '') return;

    const input = document.getElementById('copilot-input');
    input.value = '';

    appendChatMessage(msg, true);

    try {
        const response = await fetch('/api/ai/chat', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ message: msg, history: chatHistory })
        });

        if (response.ok) {
            const data = await response.json();
            appendChatMessage(data.response, false);
            chatHistory.push({ text: msg, isUser: true });
            chatHistory.push({ text: data.response, isUser: false });
        } else {
            appendChatMessage("I encountered an error communicating with the agentic POS service.", false);
        }
    } catch (e) {
        // Fallback demo response if LLM / Ollama server is offline
        const mockResponse = getMockAiResponse(msg);
        appendChatMessage(mockResponse, false);
    }
}

function appendChatMessage(text, isUser) {
    const log = document.getElementById('copilot-chat-log');
    const msg = document.createElement('div');
    msg.className = `chat-message ${isUser ? 'user' : 'assistant'}`;
    msg.textContent = text;
    log.appendChild(msg);
    log.scrollTop = log.scrollHeight;
}

function getMockAiResponse(msg) {
    const txt = msg.toLowerCase();
    if (txt.includes('inventory') || txt.includes('keyboard') || txt.includes('mouse')) {
        return "✦ [AI Agent Tool Execution: GetInventoryStatus]\n\n" +
               "Here is the current stock level for seeded items:\n" +
               "- **Wireless Mouse** (SKU-MOUSE-100): 20 units available (Reorder level: 5)\n" +
               "- **Mechanical Keyboard** (SKU-KEYBOARD-200): 10 units available (Reorder level: 3)\n\n" +
               "Both items are currently healthy.";
    }
    if (txt.includes('sales') || txt.includes('today') || txt.includes('summary')) {
        return "✦ [AI Agent Tool Execution: GetSalesSummary]\n\n" +
               "Here is the sales summary for the past 24 hours:\n" +
               "- **Total Revenue**: $1,240.50\n" +
               "- **Transactions**: 18 checkouts\n" +
               "- **Average Sale**: $68.90";
    }
    if (txt.includes('purchase') || txt.includes('draft') || txt.includes('restock')) {
        return "✦ [AI Agent Tool Execution: DraftPurchaseOrder]\n\n" +
               "I scanned the inventory and noted all stock levels are healthy. No items are below the reorder threshold right now, so no purchase order restock is required.";
    }
    return "I am your NETPOS Copilot. I can help you draft purchase orders, check item catalog status, or calculate sales revenue summaries.";
}
