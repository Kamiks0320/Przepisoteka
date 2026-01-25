import { useEffect, useState } from "react";

export function useAuth() {
    const [token, setToken] = useState<string | null>(null);

    useEffect(() => {
        const storedToken = localStorage.getItem("token");
        setToken(storedToken);
    }, []);

    function logout() {
        localStorage.removeItem("token");
        setToken(null);
        window.location.href = "/login";
    }

    function parseJwt(token: string) {
        return JSON.parse(atob(token.split(".")[1]));
    }

    const payload = token ? parseJwt(token) : null;

    const userId = payload?.sub ?? null;

    const roleClaim =
      payload?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    
    const roles = roleClaim
      ? Array.isArray(roleClaim)
          ? roleClaim
          : [roleClaim]
      : [];


    return {
        token,
        isLoggedIn: !!token,
        userId,
        roles,
        logout
    };
}
