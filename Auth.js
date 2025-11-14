const loginModal = new bootstrap.Modal(document.getElementById("loginModal"));
const registerModal = new bootstrap.Modal(document.getElementById("registerModal"));

function validatePassword(password) {
    const minLength = 6;
    const requireUpper = true;
    const requireLower = true;
    const requireDigit = true;
    const requireSpecial = true;

    if (password.length < minLength) return false;
    if (requireUpper && !/[A-Z]/.test(password)) return false;
    if (requireLower && !/[a-z]/.test(password)) return false;
    if (requireDigit && !/\d/.test(password)) return false;
    if (requireSpecial && !/[^A-Za-z0-9]/.test(password)) return false;

    return true;
}

async function login() {
    const email = document.getElementById("loginEmail").value;
    const password = document.getElementById("loginPassword").value;
    
    try 
    {
        if (!email || !password) 
            {
            Swal.fire('Error', 'Please enter both email and password!', 'error');
            return;
        }
        const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailPattern.test(email)) 
        {
            Swal.fire('Error', 'Please enter a valid email!', 'error');
            return;
        }
        const response = await axios.post("https://localhost:7181/api/Auth/login", { email, password });
        const token = response.data.token;
        const roles = response.data.roles;
        localStorage.setItem("token", token);
        localStorage.setItem("roles", JSON.stringify(roles));
        updateAuthUI(true)
        updateCRUDUI();
        loginModal.hide();
        getList();
        Swal.fire({
            icon: "success",
            title: "Logged in successfully!",
            toast: true,
            position: "top-end",
            showConfirmButton: false,
            timer: 1500,
            timerProgressBar: true,
        });
    }
    catch (error) {
        Swal.fire("Error", "Login failed", "error");
        console.error(error);
    }
}

async function register() {
    const email = document.getElementById("registerEmail").value;
    const password = document.getElementById("registerPassword").value;

    try 
    {
        if (!email || !password) 
        {
            Swal.fire('Error', 'Please enter both email and password', 'error');
            return;
        }
        const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailPattern.test(email)) 
        {
            Swal.fire('Error', 'Please enter a valid email', 'error');
            return;
        }
        if (!validatePassword(password)) {
        Swal.fire({
            icon: 'warning',
            title: 'Password Requirement',
            html: 'Password must be at least 6 characters, include uppercase, lowercase, a number and a special character.'
        });
        return;
    }
        await axios.post("https://localhost:7181/api/Auth/register", { email, password });
        Swal.fire({
            icon: "success",
            title: "Registered successfully!",
            toast: true,
            position: "top-end",
            showConfirmButton: false,
            timer: 1500,
            timerProgressBar: true,
        });
        switchToLogin();
    } 
    catch (error) 
    {
        Swal.fire("Error", "Registration failed", "error");
        console.error(error);
    }
}

function openLoginModal() {
    registerModal.hide();

    loginModal.show();

    document.getElementById("loginEmail").value = "";
    document.getElementById("loginPassword").value = "";
}

function switchToRegister() {
    loginModal.hide();
    registerModal.show();
}

function switchToLogin() {
    registerModal.hide();
    loginModal.show();
}

async function logout() {
    const result = await Swal.fire({
        title: "Are you sure?",
        text: "Do you really want to log out?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, log out",
        cancelButtonText: "Cancel",
        confirmButtonColor: "#d33",
        cancelButtonColor: "#3085d6"
    });

    if (result.isConfirmed)
    {
        localStorage.removeItem("token");
        localStorage.removeItem("roles");

        updateAuthUI(false);
        updateCRUDUI();
        Swal.fire({
            icon: "info",
            title: "You have been logged out!",
            toast: true,
            position: "top-end",
            showConfirmButton: false,
            timer: 1500,
            timerProgressBar: true,
        });

        document.getElementById("todo-table").innerHTML = '<tr><td colspan="3">Please log in to view your list</td></tr>';
    }
}

function updateAuthUI(isLoggedIn) {
    const btnLogin = document.getElementById("btnLogin");
    const btnLogout = document.getElementById("btnLogout");
    const btnCreate = document.getElementById("btnCreate");
    if (isLoggedIn) {
        btnLogin.classList.add("d-none");
        btnLogout.classList.remove("d-none");
    } else {
        btnLogin.classList.remove("d-none");
        btnLogout.classList.add("d-none");
    }
}