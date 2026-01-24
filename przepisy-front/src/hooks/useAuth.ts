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

    return {
        token,
        isLoggedIn: !!token,
        logout
    };
}
