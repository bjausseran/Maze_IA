<?php

namespace App\Http\Middleware;

use Illuminate\Foundation\Http\Middleware\VerifyCsrfToken as Middleware;

class VerifyCsrfToken extends Middleware
{
    /**
     * The URIs that should be excluded from CSRF verification.
     *
     * @var array
     */
    protected $except = [
        'maze/*',
        'user/*',
        'http://mazelaravel.test/api/maze',
        'http://mazelaravel.test/api/user',
    ];
}
