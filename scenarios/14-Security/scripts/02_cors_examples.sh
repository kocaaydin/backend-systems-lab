#!/usr/bin/env bash
set -e

echo "[BAD] AllowAnyOrigin + AllowCredentials"
cat <<'BAD'
services.AddCors(o => o.AddPolicy("bad", p =>
  p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials()));
BAD

echo
echo "[GOOD] Explicit origin list"
cat <<'GOOD'
services.AddCors(o => o.AddPolicy("good", p =>
  p.WithOrigins("https://app.company.com")
   .WithMethods("GET","POST")
   .WithHeaders("Authorization","Content-Type")
   .AllowCredentials()));
GOOD
