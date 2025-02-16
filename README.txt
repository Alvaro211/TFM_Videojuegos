Metricas Plataformas:
	En X para plataformas pequeñas 7 unidades
	En Y para cambiar altura, 2/0/-2
	En Z para diferentes filas 12/0/-12
	Las luces tienes que estar a 4 unidades de la plataforma


Para crear los Hotspot, en el script HotSpot deben declararse varias variables:

	TiempoLucesEncendidas, duración de las luces antes de que empiecen empiecen a perder intensidad.

	FadeDuration, duración durante la cual pierden intensidad las luces.

	NumLightPlatform, es un vector donde hay que poner las luces que tiene cada plataforma, hay que poner el número de luces de la plataforma más cercana a la más lejana.

	AllLight, hay que poner todas las luces a las que afecta el hotspot, las primeras luces son las que más tiempo van a estar encendidas, es decir las luces más cercanas.
