import Link from "next/link";

export default function Navbar() {
    return (
        <header className="h-14 border-b">
            <nav className="max-w-6xl mx-auto flex items-center justify-between px-6 h-full">
                <Link href="/" className="text-lg-font-semibold">Przepisoteka</Link>

                <div className="flex gap-4">
                    <Link href="/login" className="hover:underline">Zaloguj się</Link>
                    <Link href="/register" className="hover:underline">Utwórz konto</Link>
                </div>
            </nav>
        </header>
    )
}