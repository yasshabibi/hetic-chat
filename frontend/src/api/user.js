export const registerUser = async (userData) => {
    const response = await fetch('https://localhost:7266/user/register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(userData),
    });

    if (!response.ok) {
        const errorBody = await response.text(); // Utilise text() pour éviter les erreurs de parsing JSON.
        throw new Error(errorBody || 'Error occurred during registration');
    }

    // Vérifie le type de contenu de la réponse avant d'essayer de parser en JSON
    const contentType = response.headers.get('content-type');
    if (contentType && contentType.includes('application/json')) {
        return await response.json();
    } else {
        // Si ce n'est pas du JSON, tu peux retourner le texte brut ou une valeur par défaut.
        console.log('Received non-JSON response');
        return await response.text();
    }
};


export const loginUser = async (username, password) => {
    const response = await fetch('https://localhost:7266/User/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ username, password }),
    });

    if (!response.ok) {
        const errorBody = await response.text();
        throw new Error(errorBody || 'Error occurred during login');
    }

    return await response.json();
};
