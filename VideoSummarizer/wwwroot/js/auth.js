$(document).ready(function () {
    // Theme switcher
    $('.theme-btn').click(function () {
        const theme = $(this).data('theme');
        $('body').attr('data-theme', theme);
        localStorage.setItem('theme', theme);
    });

    // Apply saved theme
    const savedTheme = localStorage.getItem('theme') || 'light';
    $('body').attr('data-theme', savedTheme);

    // Form toggler
    $("#intoSignInPageBtn, #intoSignUpPageBtn").click(function (e) {
        e.preventDefault();
        toggleForms();
    });

    // Login form submission
    $("#loginForm").submit(function (e) {
        e.preventDefault();
        sendAuthData();
    });

    // Register form submission
    $("#registerForm").submit(function (e) {
        e.preventDefault();
        sendRegisterData();
    });

    function toggleForms() {
        const leftLogin = $("#left-login-content");
        const leftRegister = $("#left-register-content");
        const rightLogin = $("#right-login-form");
        const rightRegister = $("#right-register-form");

        leftLogin.toggleClass("hidden-form");
        leftRegister.toggleClass("hidden-form");
        rightLogin.toggleClass("hidden-form");
        rightRegister.toggleClass("hidden-form");

        // Animation
        if (leftLogin.hasClass("hidden-form")) {
            animateTransition(leftRegister, rightRegister);
        } else {
            animateTransition(leftLogin, rightLogin);
        }
    }

    function animateTransition(left, right) {
        left.css({ opacity: 0, transform: "translateX(-20px)" });
        right.css({ opacity: 0, transform: "translateX(20px)" });
        
        left.animate({ opacity: 1, transform: "translateX(0)" }, 500);
        right.animate({ opacity: 1, transform: "translateX(0)" }, 500);
    }

    async function sendAuthData() {
        const login = $("#login").val();
        const password = $("#password").val();

        if (!login || !password) {
            showNotification("Please enter login and password", "error");
            return;
        }

        const formData = new FormData();
        formData.append('login', login);
        formData.append('password', password);

        $("#logInBtn").prop("disabled", true).text("Signing In...");

        try {
            const response = await fetch('/Authorization/SignIn', {
                method: 'POST',
                body: formData
            });

            if (response.ok) {
                showNotification("Login successful! Redirecting...", "success");
                setTimeout(() => {
                    window.location.href = '/';
                }, 1500);
            } else if (response.status === 403) {
                const data = await response.json();
                showNotification(data || "Invalid login or password", "error");
            } else {
                const data = await response.json();
                showNotification(data || "Login failed", "error");
            }
        } catch (error) {
            showNotification("Network error. Please try again.", "error");
        } finally {
            $("#logInBtn").prop("disabled", false).text("Sign In");
        }
    }

    async function sendRegisterData() {
        const username = $("#username").val();
        const login = $("#reg-login").val();
        const password = $("#reg-password").val();
        const confirmPassword = $("#confirm-password").val();

        if (!username || !login || !password || !confirmPassword) {
            showNotification("Please fill all fields", "error");
            return;
        }

        if (password !== confirmPassword) {
            showNotification("Passwords don't match", "error");
            return;
        }

        const formData = new FormData();
        formData.append('Username', username);
        formData.append('Login', login);
        formData.append('Password', password);
        formData.append('ConfirmPassword', confirmPassword);

        $("#registerBtn").prop("disabled", true).text("Creating Account...");

        try {
            const response = await fetch('/Authorization/Register', {
                method: 'POST',
                body: formData
            });

            if (response.ok) {
                showNotification("Registration successful! Please sign in.", "success");
                setTimeout(() => {
                    toggleForms();
                }, 1500);
            } else {
                const data = await response.json();
                showNotification(data || "Registration failed", "error");
            }
        } catch (error) {
            showNotification("Network error. Please try again.", "error");
        } finally {
            $("#registerBtn").prop("disabled", false).text("Sign Up");
        }
    }

    function showNotification(message, type) {
        const notification = $("#notification");
        const notificationText = $("#notification-text");
        
        notification.removeClass("success error");
        notification.addClass(type);
        notificationText.text(message);
        
        notification.fadeIn(300).delay(3000).fadeOut(300);
    }
});