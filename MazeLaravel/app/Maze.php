<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class NameObj extends Model
{
    public $id;
    public $name;

    public function setId(int $id)
    {
        $this->$id = $id;
    }
    public function setName(string $name)
    {
        $this->$name = $name;
    }
}

class NameList extends Model
{
    public $nameList;
}
class Maze extends Model
{

    public function user()
    {
        return $this->belongsTo('App\User');
    }
}
