function backToTop()
{
    window.scrollTo({ top: 0, behavior: 'smooth' });
}

function focusElement(id) {
    try {
        const element = document.getElementById(id);
        element.focus();
    }
    catch {
        console.log(`Could not focus on element: ${id}.`);
    }
}