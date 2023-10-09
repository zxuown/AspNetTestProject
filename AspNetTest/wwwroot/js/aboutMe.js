const results = $('ul#search-result')
function updateMe(query) {
    fetch("/Home/Abilities", {
        method: "post",
        headers: {
            'Content-Type': "application/json"
        },
        body: JSON.stringify({
            Query: query
        })
    }).then(r => r.json()).then(abilities => {
        results.empty()
        abilities.forEach(c => {
            results.append(`<li class="list-group-item list-group-item-dark">${c.name}<div class="progress-bar progress-bar-striped" role="progressbar" style="width: ${c.level}%; height: 20px"
	  aria-valuenow="${c.level}" aria-valuemin="0" aria-valuemax="100"></div></li>`)
        })
    })
}
$("input#search").on('input', e => {
    updateMe($(e.target).val()) 
})
updateMe("")
