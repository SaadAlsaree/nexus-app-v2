'use client';

import { useTheme } from 'next-themes';
import React from 'react';
import { ActiveThemeProvider } from '../active-theme';
import { DirectionProvider } from '@radix-ui/react-direction';

export default function Providers({
  activeThemeValue,
  children
}: {
  activeThemeValue: string;
  children: React.ReactNode;
}) {
  const { resolvedTheme } = useTheme();

  return (
    <>
      <ActiveThemeProvider initialTheme={activeThemeValue}>
        <DirectionProvider dir='rtl'>{children}</DirectionProvider>
      </ActiveThemeProvider>
    </>
  );
}
