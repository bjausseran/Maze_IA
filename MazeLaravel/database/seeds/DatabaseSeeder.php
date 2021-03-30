<?php

use Illuminate\Database\Seeder;

class DatabaseSeeder extends Seeder
{
    /**
     * Seed the application's database.
     *
     * @return void
     */
    public function run()
    {
        $this->call(UserSeeder::class);
        $this->call(MazeSeeder::class);
        $this->call(TileSeeder::class);
        $this->call(TestSeeder::class);
    }
}
