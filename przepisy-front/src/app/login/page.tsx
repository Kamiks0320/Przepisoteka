'use client'

import { useRouter } from "next/navigation";
import { useState } from "react";

export default function LoginPage() {
    const router = useRouter();
    
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState<string | null>(null);
    
    async function handleSubmit(e: React.FormEvent){
        e.preventDefault();
        setError(null);

        if(!email || !password) {
            setError("Email i hasło są wymagane");
            return;
        }

        const res = await fetch("http://localhost:5220/api/auth/login", {
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({email, password})
        });

        if(!res.ok) {
            const message = await res.text();
            setError(message || "Błąd logowania");
            return;
        }

        const data = await res.json();

        localStorage.setItem("token", data.token);

        router.push("/")
    }

    return (
        <form className="max-w-md mx-auto mt-10 space-y-4">
            <h1 className="text-2xl font bold">Zaloguj się</h1>

            <input placeholder="Email" value={email} onChange={e => setEmail(e.target.value)} className="border p-2 w-full" />
            <input type="password" placeholder="Hasło" value={password} onChange={e => setPassword(e.target.value)} className="border p-2 w-full" />

            <button onClick={handleSubmit} className="bg-blue-600 text-white px-4 py-2 rounded">Zaloguj się</button>
        </form>
    )
}