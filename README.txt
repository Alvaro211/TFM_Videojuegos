Métricas Plataformas:
	La escala mínima en Y es 1
	La escala en Z es 10
	La posición en Y  entre las plataformas son 3

Métricas Luces Plataforma:
	La altura a la plataforma que ilumina es 2
	Debe haber 2 luces para iluminar
	Ambas luces deben estar en Z  0.25 y -0.25

Jugador:
	Salto cómodo longuitud:	10
	Salto máximo longitud:	14
	Salto cómodo altura: 	3
	Salto máxima altura:	4
	Salto cómodo longitud diferente altura las plataformas:	8
	Salto máximo longitud diferente altura las plataformas:	12





Para crear los Hotspot, en el script HotSpot deben declararse varias variables:

	TiempoLucesEncendidas, duración de las luces antes de que empiecen empiecen a perder intensidad.

	FadeDuration, duración durante la cual pierden intensidad las luces.

	NumLightPlatform, es un vector donde hay que poner las luces que tiene cada plataforma, hay que poner el número de luces de la plataforma más cercana a la más lejana.

	AllLight, hay que poner todas las luces a las que afecta el hotspot, las primeras luces son las que más tiempo van a estar encendidas, es decir las luces más cercanas.
