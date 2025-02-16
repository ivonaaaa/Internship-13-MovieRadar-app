import { removeAuthToken } from "./auth.js";
import { removeIsAdmin } from "./auth.js";

function logoutUser() {
  removeAuthToken();
  removeIsAdmin();
  window.location.href = "./index.html";
}

export function createLogoutButton() {
  const logoutButton = document.createElement("button");
  logoutButton.textContent = "Logout";

  logoutButton.style.position = "fixed";
  logoutButton.style.top = "10px";
  logoutButton.style.right = "10px";
  logoutButton.style.padding = "5px 10px";
  logoutButton.style.fontSize = "12px";
  logoutButton.style.zIndex = "1000";
  logoutButton.style.display = "inline-block";
  logoutButton.style.width = "auto";
  logoutButton.style.cursor = "pointer";

  logoutButton.classList.add("logout-button");
  logoutButton.addEventListener("click", logoutUser);
  return logoutButton;
}
