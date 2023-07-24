import React from 'react'

export default function Page({ params }: any ) {
  return (
    <div>page with { params.id }, type { typeof(params) }</div>
  )
}

