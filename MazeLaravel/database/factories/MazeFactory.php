<?php

/** @var \Illuminate\Database\Eloquent\Factory $factory */

use App\Maze;
use App\User;
use Faker\Generator as Faker;

$factory->define(Maze::class, function (Faker $faker) {

    $userId = $faker->randomElement(User::pluck('id')->all());
    $name = $faker->lastName;

    return [
        'name' => $name,
        'user_id' => $userId,
        'composition' => $faker->text($maxNbChars = 2000),
    ];
});
