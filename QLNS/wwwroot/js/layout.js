/**
 * Layout and Sidebar Management
 * Handles consistent behavior of the sidebar across all pages
 */

document.addEventListener("DOMContentLoaded", function () {
    updateSidebar();
    setupLogout();
});

/**
 * Update sidebar items visibility based on user role
 */
function updateSidebar() {
    // Only proceed if authenticated (auth.js functions should be available)
    if (typeof isAuthenticated !== 'function' || !isAuthenticated()) {
        return;
    }

    // Check for admin role (1 = Admin)
    const isAdmin = typeof hasRole === 'function' && hasRole(1);

    if (!isAdmin) {
        const restrictedItems = document.querySelectorAll('.restricted.admin-only');
        restrictedItems.forEach(item => {
            item.style.display = 'none';
            // Also add hidden class to be sure
            item.classList.add('hidden');
        });
    }
}

/**
 * Setup logout button handler if it exists
 */
function setupLogout() {
    // We delegate the click event or attach it if the element exists
    // Most pages use inline onclick="handleLogout()", but we can also attach it here if needed.
    // Index.js has handleLogout defined globaly.
    // For subpages, we might need a shared handleLogout if they don't have their own script defining it.
}

// Subpages might need a shared logout function if they don't refer to index.js
if (typeof window.handleLogout === 'undefined') {
    window.handleLogout = function () {
        if (confirm("Bạn có chắc chắn muốn đăng xuất?")) {
            if (typeof logout === 'function') {
                logout();
            } else {
                // Fallback if auth.js not loaded/working
                localStorage.removeItem("token");
                window.location.href = "/pages/login.html"; // Adjust path if needed, usually /pages/login.html
            }
        }
    };
}
