"use client"

import {useState} from "react";
import { useRouter } from "next/navigation";

export default function RegisterPage() {
    const router = useRouter();

    const [email, setEmail] = useState("");
    const [nick, setNick] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [error, setError] = useState<string | null>(null);

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        setError(null);

        if(!email || !nick || !password) {
            setError("Wszystkie pola są wymagane");
            return;
        }

        if(password !== confirmPassword) {
            setError("Hasła się różnią od siebie");
            return;
        }

        const res = await fetch(("http://localhost:5220/api/auth/register"), {
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({email, nick, password})
        });

        if(!res.ok) {
            const message = await res.text();
            setError(message || "Błąd rejestracji");
            return;
        }

        router.push(`/login`);
    }

    return(
        <form className="max-w-md mx-auto mt-10 space-y-4">
            <h1 className="text-2xl font-bold">Utwórz konto</h1>

            <input placeholder="Email" value={email} onChange={e => setEmail(e.target.value)} className="border p-2 w-full" />
            <input placeholder="Nazwa użytkownika" value={nick} onChange={e => setNick(e.target.value)} className="border p-2 w-full" />
            <input type="password" placeholder="Hasło" value={password} onChange={e => setPassword(e.target.value)} className="border p-2 w-full" />
            <input type="password" placeholder="Potwierdź hasło" value={confirmPassword} onChange={e => setConfirmPassword(e.target.value)} className="border p-2 w-full" />

            {error && <p className="text-red-600">{error}</p>}

            <button onClick={handleSubmit} className="bg-blue-600 text-white px-4 py-2 rounded">Zarejestruj się</button>
        </form>
    )
}