var map = L.map('map').setView([48.621115, 22.287735], 13);

L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
}).addTo(map);

let markers = [];
function loadMarkers(map) {
    fetch("/Map/Markers").then(r => r.json()).then(list => {
        
        list.forEach(markerData => {
            //console.log(markerData)
            //console.log(map)
            let marker = L.marker([Number(markerData.lat), Number(markerData.lon)], {draggable : true, autoPan: true}).addTo(map);
            marker.on('click', e => {
                let dbMarker = e.target.dbMarker;
                Swal.fire({
                    title: 'Edit Marker txt:',
                    input: 'text',
                    inputValue: dbMarker.name,
                    showCancelButton: true,
                    confirmButtonText: 'Save',
                    showDenyButton: true,
                    denyButtonText: 'Delete',
                    cancelButtonText: 'Cancel',
                }).then((result) => {
                    if (result.isConfirmed) {
                        e.target.bindPopup(result.value).openPopup();
                        dbMarker.name = result.value
                        fetch(`/Map/Edit/${dbMarker.id}`, {
                            method: "Post",
                            headers: {
                                'Content-Type': "application/json"
                            },
                            body: JSON.stringify(dbMarker)
                        })  
                    }
                    if (result.isDenied) {
                        fetch(`/Map/Delete/${dbMarker.id}`, {
                            method: "Delete",
                        }).then(() => {
                            map.removeLayer(e.target)
                        })
                    }
                });
            })
            //marker.on('move', e => console.log(e))
            marker.on('dragend', e => {
                console.log(e)
                let dbMarker = e.target.dbMarker;
                dbMarker.lon = String(e.target.getLatLng().lng)
                dbMarker.lat = String(e.target.getLatLng().lat)
                fetch(`/Map/Edit/${dbMarker.id}`, {
                    method: "Post",
                    headers: {
                        'Content-Type': "application/json"
                    },
                    body: JSON.stringify(dbMarker)
                })
            })
            //console.log(marker)
            marker.bindPopup(markerData.name);
            marker.dbMarker = markerData
            markers.push(marker)
        });
    })
}
//if (localStorage.getItem('markers')) {
//    markers = JSON.parse(localStorage.getItem('markers'));

//    markers.forEach(markerData => {
//        var marker = L.marker(markerData.latlng).addTo(map);
//        marker.bindPopup(markerData.name).openPopup();
//    });
//}

function onMapClick(e) {
    Swal.fire({
        title: 'Enter marker name:',
        input: 'text',
        showCancelButton: true,
        confirmButtonText: 'OK',
        cancelButtonText: 'Cancel',
    }).then((result) => {
        if (result.isConfirmed) {
            var marker = L.marker(e.latlng).addTo(map);
            marker.bindPopup(result.value).openPopup();
            fetch("/Map/Create", {
                method: "Post",
                headers: {
                    'Content-Type': "application/json"
                },
                body: JSON.stringify({
                    Name: result.value,
	                Lat: String(e.latlng.lat),
                    Lon: String(e.latlng.lng),
                })
                })
            markers.push({ name: result.value, latlng: marker.getLatLng() });
            localStorage.setItem('markers', JSON.stringify(markers));
        }
    });
}

map.on('click', onMapClick);
loadMarkers(map);