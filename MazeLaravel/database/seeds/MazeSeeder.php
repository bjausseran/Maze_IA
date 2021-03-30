<?php

use Illuminate\Database\Seeder;

class MazeSeeder extends Seeder
{
    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {
        factory(App\Maze::class, 50)->create();
    }
}
