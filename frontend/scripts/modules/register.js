
import { generateJWT, saveAuthToken } from "./auth.js";

document.addEventListener("DOMContentLoaded", function () {
  const registerContainer = document.getElementById("register-container");
  if (!registerContainer) return;

  const registerButton = registerContainer.querySelector("button");
  registerButton.addEventListener("click", function () {
    const usernameInput = registerContainer.querySelector("input[placeholder='Username']");
    const emailInput = registerContainer.querySelector("input[placeholder='Email']");
    const passwordInput = registerContainer.querySelector("input[placeholder='Password']");

    const username = usernameInput.value.trim();
    const email = emailInput.value.trim();
    const password = passwordInput.value.trim();

    if (!username || !email || !password) {
      alert("Sva polja su obavezna.");
      return;
    }

    
    //koristenje localstorage za test
    let registeredUsers = JSON.parse(localStorage.getItem("registeredUsers")) || [];
    const emailExists = registeredUsers.some(u => u.email === email);
    if (emailExists) {
      alert("Korisnik s tim emailom već postoji.");
      return;
    }

    const newUser = {
      id: Date.now(),
      username,
      email,
      password,
      is_admin: false,
    };

    registeredUsers.push(newUser);
    localStorage.setItem("registeredUsers", JSON.stringify(registeredUsers));

    const token = generateJWT(newUser);
    saveAuthToken(token);

    window.location.href = "ivona-test.html";
  });
});
