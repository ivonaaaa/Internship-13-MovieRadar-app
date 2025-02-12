
export function setCookie(name, value, minutes) {
    let expires = "";
    if (minutes) {
      const date = new Date();
      date.setTime(date.getTime() + minutes * 60 * 1000);
      expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
  }
  
  export function getCookie(name) {
    const nameEQ = name + "=";
    const ca = document.cookie.split(";");
    for (let i = 0; i < ca.length; i++) {
      let c = ca[i];
      while (c.charAt(0) === " ") c = c.substring(1, c.length);
      if (c.indexOf(nameEQ) === 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
  }
  
  export function eraseCookie(name) {
    document.cookie = name + "=; Max-Age=-99999999; path=/";
  }
  
  export function generateJWT(user) {
    const payload = {
      id: user.id,
      username: user.username,
      email: user.email,
      is_admin: user.is_admin,
      exp: Date.now() + 30 * 60 * 1000, // token traje 30 minuta
    };
    return btoa(JSON.stringify(payload));
  }
  
  export function saveAuthToken(token) {
    setCookie("authToken", token, 30);
  }
  
  export function getAuthToken() {
    return getCookie("authToken");
  }
  
  export function removeAuthToken() {
    eraseCookie("authToken");
  }
  
  export function decodeJWT(token) {
    try {
      return JSON.parse(atob(token));
    } catch (error) {
      console.error("Invalid token", error);
      return null;
    }
  }
  