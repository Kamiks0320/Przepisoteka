"use client"

import Link from "next/link";
import {useAuth} from "@/hooks/useAuth";

export default function AddRecipeButton() {
    const {isLoggedIn} = useAuth();

    if(!isLoggedIn) return null;
    
    return(
            <Link href={`/recipes/add`}>Dodaj przepis</Link>
    )
}