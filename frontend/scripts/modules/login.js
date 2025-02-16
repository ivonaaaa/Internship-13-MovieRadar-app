import { saveAuthToken, decodeToken, getAuthToken } from "./auth.js";
import { initAdminApp } from "./admin-app.js";
import { initUserApp } from "./user-app.js";
import { loginUser } from "../api/api.js";
import { setIsAdmin } from "./authState.js";
import { getIsAdmin } from "./auth.js";
import { saveIsAdmin } from "./auth.js";

document.addEventListener("DOMContentLoaded", function () {
  const loginContainer = document.getElementById("login-container");

  const token = getAuthToken();
  const isAdmin = getIsAdmin();

  if (token) {
    loginContainer.style.display = "none";
    isAdmin ? initAdminApp() : initUserApp();
    return;
  }

  if (!loginContainer) return;

  const loginButton = loginContainer.querySelector("button");
  loginButton.addEventListener("click", async function () {
    const usernameInput = loginContainer.querySelector(
      "input[placeholder='Username']"
    );
    const passwordInput = loginContainer.querySelector(
      "input[placeholder='Password']"
    );
    const email = usernameInput.value.trim();
    const password = passwordInput.value.trim();

    if (!email || !password) {
      alert("Please enter email and password.");
      return;
    }

    try {
      const data = await loginUser(email, password);

      if (!data || !data.token || typeof data.isAdmin === "undefined")
        throw new Error("Invalid response from server.");

      const { token, isAdmin } = data;

      saveAuthToken(token);

      setIsAdmin(isAdmin);
      saveIsAdmin(isAdmin);

      loginContainer.style.display = "none";

      isAdmin ? initAdminApp() : initUserApp();
    } catch (error) {
      alert(error.message);
    }
  });

  const createBtn = document.getElementById("create-btn");
  if (createBtn) {
    createBtn.addEventListener("click", function () {
      loginContainer.style.display = "none";
      const registerContainer = document.getElementById("register-container");
      if (registerContainer) registerContainer.style.display = "block";
    });
  }
});
