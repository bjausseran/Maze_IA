<?php

use Illuminate\Database\Seeder;

class TileSeeder extends Seeder
{
    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {
        for($i = 0; $i < 8; ++$i) {
            factory(App\Tile::class, 1)->create();
    }
    }
}
