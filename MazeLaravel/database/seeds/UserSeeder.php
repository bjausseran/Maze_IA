<?php

use Illuminate\Database\Seeder;

class UserSeeder extends Seeder
{
    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {
        DB::table('users')->insert([
            'name' => 'Benoit',
            'status' => 'admin',
            'email' => 'admin1@maze.com',
            'email_verified_at' => now(),
            'password' => 'admin', // password
            'api_token' => 'admin001',
            'remember_token' => Str::random(10),

        ]);
        DB::table('users')->insert([
            'name' => 'Aymeric',
            'status' => 'admin',
            'email' => 'admin2@maze.com',
            'email_verified_at' => now(),
            'password' => 'admin', // password
            'api_token' => 'admin002',
            'remember_token' => Str::random(10),

        ]);
        factory(App\User::class, 10)->create();
    }
}
